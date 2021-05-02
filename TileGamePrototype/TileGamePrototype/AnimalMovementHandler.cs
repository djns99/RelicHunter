using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileGamePrototype
{
    static class AnimalMovementHandler
    {
        public delegate void movementHandlerCallBack(bool reverse);
        public delegate void movementHandlerDeathCallBack();
        static List<Request> requests = new List<Request>();

        //Add a request to the list of requests
        public static void requestMovement(Request request)
        {
            requests.Add(request);
        }

        public static void handleAllRequests()
        {
            //Exit if there is only one request
            if (requests.Count == 0)
            {
                requests = new List<Request>();
                return;
            }
            //Use simpler code for single request
            if (requests.Count <= 1)
            {
                if (levelHandler.currentLevelSurfaceArray[requests[0].newLoc.X, requests[0].newLoc.Y].isWalkable)
                {
                    requests[0].callback(false);
                }
                else if (levelHandler.currentLevelSurfaceArray[requests[0].altLoc.X, requests[0].altLoc.Y].isWalkable)
                {
                    requests[0].callback(true);
                }
                requests = new List<Request>();
                return;
                //By not responding permission is assumed to be refused and current state is maintained
            }


            ////////////If it goes past here, your computer my seize up in revulsion. Its screwed. All of it. (Also I have forgotten how it works and didn't comment it enough...)


            //List of requests that have valid moves
            List<Request> toCallBack = new List<Request>();
            //Dictionary containing all the requests to move and their relative location
            Dictionary<System.Drawing.Point, LocationRequests> locationsInvolved = new Dictionary<System.Drawing.Point, LocationRequests>();

            //Add all the requests to the Location request dictionary
            foreach (Request request in requests)
            {
                addRequestToDictionary(locationsInvolved, request, request.newLoc);
            }

            int incomplete = locationsInvolved.Count;

            //Iterate through each request in the dictionary
            do
            {
                incomplete = 0;
                foreach (KeyValuePair<System.Drawing.Point, LocationRequests> dictionaryEntry in locationsInvolved)
                {
                    LocationRequests curLocRequest = dictionaryEntry.Value;
                    //There are multiple request to move to this location
                    if (curLocRequest.requestsToMove.Count > 1)
                    {
                        int priority = curLocRequest.requestsToMove[0].priority;
                        bool priorityDiff = false;
                        for (int i = curLocRequest.requestsToMove.Count - 1; i >= 0; i--)
                        {
                            Request request = curLocRequest.requestsToMove[i];
                            if (request.reversed == true)//Override if it has been reversed. Includes hostile animals - though they will kill it if it moves there later
                            {
                                curLocRequest.requestsToMove.Remove(request);
                            }
                            if (priority != request.priority)
                            {
                                priorityDiff = true;
                                priority = Math.Max(priority, request.priority);
                            }

                        }
                        if (priorityDiff)
                        {
                            for (int i = curLocRequest.requestsToMove.Count - 1; i >= 0; i--)
                            {
                                Request request = curLocRequest.requestsToMove[i];
                                if (priority > request.priority)
                                {
                                    request.die();//kills animal

                                    //Remove it from location
                                    curLocRequest.requestsToMove.Remove(request);
                                    if (locationsInvolved.ContainsKey(request.curLoc))
                                    {
                                        LocationRequests locRequest = locationsInvolved[request.curLoc];
                                        locRequest.occupied = false;
                                    }
                                }
                            }
                            if (curLocRequest.requestsToMove.Count > 1)
                            {
                                //Turn the remaining animals around
                                reverseRequest(curLocRequest, locationsInvolved);
                            }
                            else
                            {
                                if (curLocRequest.occupied == false && curLocRequest.requestsToMove.Count > 0)
                                {
                                    toCallBack.Add(curLocRequest.requestsToMove[0]);
                                    curLocRequest.completed = true;
                                }
                            }
                        }
                        else
                        {
                            //If they have same priority, reject them all
                            reverseRequest(curLocRequest, locationsInvolved);
                            curLocRequest.completed = true;//Will not accept any other movement requests. Animals have escaped if predator alternate position
                        }
                    }
                    //There is a single request to move to this location
                    else if (curLocRequest.requestsToMove.Count == 1)
                    {
                        if (curLocRequest.occupied == false)
                        {
                            toCallBack.Add(curLocRequest.requestsToMove[0]);
                            curLocRequest.occupied = true;
                            curLocRequest.completed = true;
                            System.Drawing.Point originalLoc = curLocRequest.requestsToMove[0].curLoc;
                            LocationRequests originalRequest = locationsInvolved[originalLoc];
                            originalRequest.occupied = false;
                            originalRequest.completed = true;
                        }
                    }
                    else
                    {
                        if (curLocRequest.occupied == false)
                        {
                            curLocRequest.completed = true; //May cause issues but is neccesary
                        }
                    }
                    if (curLocRequest.completed == false)
                    {
                        incomplete++;
                    }
                }
            } while (incomplete > 0);
            //Clean slate for requests
            requests = new List<Request>();
        }

        //Reverse in the opposite direction
        private static void reverseRequest(LocationRequests locRequest,  Dictionary<System.Drawing.Point, LocationRequests> locationsInvolved)
        {
            for (int i = locRequest.requestsToMove.Count - 1; i >= 0; i--)
            {
                Request request = locRequest.requestsToMove[i];
                locRequest.requestsToMove.Remove(request);
                if (request.reversed == false)
                {
                    addRequestToDictionary(locationsInvolved, request, request.altLoc);
                    request.reversed = true;
                }
                else
                {
                    if (locationsInvolved.ContainsKey(request.curLoc))
                    {
                        LocationRequests newLocRequest = locationsInvolved[request.curLoc];
                        newLocRequest.completed = true;///Cannot be moved from current location so abort all attempts to move here
                        reverseRequest(newLocRequest, locationsInvolved);
                    }
                }
            }
        }

        private static void addRequestToDictionary(Dictionary<System.Drawing.Point, LocationRequests> locationsInvolved, Request request, System.Drawing.Point moveLoc)
        {
            //Add Current Location and Set it to occupied
            if (!locationsInvolved.ContainsKey(request.curLoc))
            {
                LocationRequests locRequest = new LocationRequests(true);
                locationsInvolved.Add(request.curLoc, locRequest);
            }
            else
            {
                LocationRequests locRequest = locationsInvolved[request.curLoc];
                locRequest.occupied = true;
            }
            //Add new location and add request to move there
            if (!locationsInvolved.ContainsKey(moveLoc))
            {
                LocationRequests locRequest = new LocationRequests(false);
                locRequest.requestsToMove.Add(request);
                locationsInvolved.Add(moveLoc, locRequest);
            }
            else
            {
                if (locationsInvolved[moveLoc].completed == false)//Check if already moved to
                {
                    LocationRequests locRequest = locationsInvolved[moveLoc];
                    locRequest.requestsToMove.Add(request);
                }
                else
                {
                    LocationRequests locRequest = locationsInvolved[request.curLoc];
                    if (moveLoc == request.altLoc)
                    {
                        locRequest.completed = true;//Occupier cannot be moved from current location so abort all attempts to move here
                        reverseRequest(locRequest, locationsInvolved);
                    }
                    else
                    {
                        addRequestToDictionary(locationsInvolved, request, request.altLoc);//Not sure if this will happen, better to be safe than sorry
                    }
                }
            }
        }


        //Topological sort from programming comp that I wrote
        /*
        static void ikea()
        {
            while (true)
            {
                int pages = Convert.ToInt32(Console.ReadLine());
                if (pages == 0)
                {
                    break;
                }
                SortedSet<int> dependencyfree = new SortedSet<int>();
                node[] nodes = new node[pages];
                for (int i = 0; i < pages; i++)
                {
                    nodes[i] = new node();
                }
                    for (int i = 0; i < pages; i++)
                    {
                        string[] pageDetailsString = Console.ReadLine().Split();
                        int numberDeps = Convert.ToInt32(pageDetailsString[0]);
                        
                        for (int j = 0; j < numberDeps; j++)
                        {
                            int nodeDepends = Convert.ToInt32(pageDetailsString[j + 1]);
                            nodes[nodeDepends].Dependencies++;
                            nodes[i].Depended.Add(nodeDepends);
                        }
                    }
                    for (int i = 0; i < pages; i++)
                    {
                        if (nodes[i].Dependencies == 0)
                        {
                            dependencyfree.Add(i);
                        }
                    }
                int pagesRemaining = pages;

                while(pagesRemaining > 0)
                {
                    if (dependencyfree.Count == 0)
                    {
                        Console.WriteLine("Tragic Failure Occured, This Set Cannot Be Completed");
                    }
                    int page = dependencyfree.First();
                    foreach (int depended in nodes[page].Depended)
                    {
                        nodes[depended].Dependencies--;
                        if (nodes[depended].Dependencies == 0)
                        {
                            dependencyfree.Add(depended);
                        }
                    }
                    pagesRemaining--;
                    Console.Write(page.ToString() + " ");
                    dependencyfree.Remove(page);
                }
                Console.WriteLine("");
            }
        }
        */
        //Privately used struct
        private struct LocationRequests
        {
            public List<Request> requestsToMove;
            public bool occupied;
            public bool completed;

            public LocationRequests(bool _occupied, bool _completed = false)
            {
                requestsToMove = new List<Request>();
                occupied = _occupied;
                completed = _completed;
            }
        }
        //Movement request
        public struct Request
        {
            public readonly System.Drawing.Point curLoc;//Current grid position
            public readonly System.Drawing.Point newLoc;//New grid position
            public readonly System.Drawing.Point altLoc;
            public movementHandlerCallBack callback;
            public movementHandlerDeathCallBack die;
            public readonly int priority; //If hostile it will kill lower priority animals
            public bool reversed;


            public Request(System.Drawing.Point _curLoc, System.Drawing.Point _newLoc, System.Drawing.Point _altLoc, movementHandlerCallBack _callback, movementHandlerDeathCallBack _die, int _priority = 0)
            {
                curLoc = _curLoc;
                newLoc = _newLoc;
                altLoc = _altLoc;
                callback = _callback;
                priority = _priority;
                die = _die;
                reversed = false;
            }
        }
    }
}

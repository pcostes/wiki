# WikiSolver
_Created for HackTj 7.5_

WikiSolver is an online application that uses bidirection breadth-first search to find the shortest path from one wikipedia page to another. A path is here defined as a series of blue links found in pages, starting on the first given page and ending on the goal. 
## Challenges
### Searching
To begin our project we had to learn the wikipedia API, but once we formatted our calls correctly it was fairly straightforward to extract the data we needed from the text. We encountered some limitations within the wikipedia API, which forced us to change the specifics of our code (namely we wanted to only use links found within certain sections of an article, but the API did not provide information on the location of links). 
Although we both had experience with the A* and BFS algorithms, we encoutered challenges implementing a bidirectional BFS while ensuring that the shortest path was always the one returned.
### Server
We had essentially zero experience with server backend and frontend, but we managed to get our application working on the server side and to except user input.
## Continuation
We could expand this application to return a larger number of paths of equal length, to only use links from within the body of the article (which is possible but would require significant modifications to our http requests)

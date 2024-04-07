# epay Coding Test

In this repository, my solutions to the epay coding test are located.

## Problem 1: Denomination Routine

denomination-routine contains the solution for this problem. The payout test data are stored inside an array and each amount is processed to find the combinations of denomination (10, 50, 100). The solution uses a recursive function to find the total combinations and display the result for each amount in the console.

## Problem 2: REST Server

rest-server and post-simulator contains the solution for this problem. RestServer implements two functions for the POST and GET methods, while PostSimulator implements the POST simulator to insert new customers.

RestServer is the solution to implement a web server for the Customer API endpoint. Once started via *dotnet run*, the Customer API endpoint is available at *http://localhost:5050/customers*. Performing a GET method will list the current customers stored in the array. The Customer array is populated from a JSON file (customers.json) based from the coding test document. Performing a POST method, together with a list of new customers, will insert them into the Customer array and eventually saved in the JSON file.

PostSimulator is the solution to implement a POST simulator based on the requirements of the coding test document. It will execute a POST method to the Customer API endpoint to insert new customers based on the criteria set.
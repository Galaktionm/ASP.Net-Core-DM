# ASP.Net-Core-DM

Mainly focused on using different tools and creating the "skeleton" of the project. Therefore simple operations (CRUD etc.) many simple operations are ommited. Gateway should
have JWT authentication, which will verify the token issued by UserService, but for development purposes it is ommited as well. 


### General idea
![ASP Net Core Diagram](https://user-images.githubusercontent.com/106910530/207526822-cb3e97dc-4d38-4f41-b818-91bb7a2f8b70.png)

### Gateway
Gateway servie has Hangfire background job running, which checks number of orders of the branches and places the next order to less busy branch. 

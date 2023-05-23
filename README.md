### Postman API Doc - https://documenter.getpostman.com/view/18462993/2s93eR4vmq
## Folder Structure

### GistBlog.API
#### Is the root directory and it contains the following directories
1. Controllers
2. Uploaded files
3. Program.cs file
4. watchdog.db dump

### GistBlog.BLL
This layer contains the application business logic. The following folder are embedded inside it:
#### Extensions
1. ApplicationBuilderExtention.cs (Contains configuration for global error handling)
2. Middlewares.cs (Here is where the registration for the app interface and services is done)
#### Services
This directory contains two directories:
1. Contracts: This directory contains our application interfaces
2. Implementation: The implementations for our interfaces

### GistBlog.DAL
This layer contains our:
1. Models directory, 
2. Migrations directory, 
3. Database configuration.
4. Exceptions for global error handler
5. Repository directory
6. Enums

## The is a an blogging API with very advanced features such as:
### Authentication Features:
1. Normal user registration
2. Login
3. Logout
4. Check user login status
5. Forgot password recovery
6. Change password
7. Admin registration
8. Create roles
9. Assign roles
10. Edit role
11. Delete role
12. Edit a user's role
13. Delete a user's role
14. Get all roles
15. Get a user role(s)
16. Get all users and roles
17. Refresh token

### Blog Features:
1. Create new blog post
2. Get all blogs posts
3. Get all users blog post
4. Get single blog post by id
5. Update a user blog post
6. Delete a blog post
7. Blog image upload

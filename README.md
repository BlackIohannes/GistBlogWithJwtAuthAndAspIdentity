### Postman API Doc - https://documenter.getpostman.com/view/18462993/2s93eR4vmq
## Folder Structure

## The GistBlog API is a three-layer architecture, with the following folders:

>### GistBlog.API - The root directory, which contains the following subdirectories:
> 
>  - Controllers - The controllers for the API endpoints
>  - Uploaded files - The directory for uploaded files
>  - Program.cs - The main application file
>  - watchdog.db dump - A dump of the database

>### GistBlog.BLL - The business logic layer, which contains the following subdirectories:
>
>  - Extensions - Contains extensions for the application
>  - Services - Contains the application services

>### GistBlog.DAL - The data access layer, which contains the following subdirectories:
> 
> - Models - The data models
> - Migrations - The database migrations
> - Exceptions - The exceptions for the global error handler
> - Repository - The repository for the data access layer
> - Enums - The enumerations for the data access layer

## Authentication Features:
> - Normal user registration - Allows users to create a new account
> - Login - Allows users to log in to their account
> - Logout - Allows users to log out of their account
> - Check user login status - Checks whether a user is logged in
> - Forgot password recovery - Allows users to reset their password
> - Change password - Allows users to change their password
> - Admin registration - Allows administrators to create an account
> - Create roles - Allows administrators to create new roles
> - Assign roles - Allows administrators to assign roles to users
> - Edit role - Allows administrators to edit roles
> - Delete role - Allows administrators to delete roles
> - Edit a user's role - Allows administrators to edit a user's role
> - Delete a user's role - Allows administrators to delete a user's role
> - Get all roles - Allows administrators to get a list of all roles
> - Get a user role(s) - Allows administrators to get a user's role(s)
> - Get all users and roles - Allows administrators to get a list of all users and their roles
> - Refresh token - Allows users to refresh their access token

## Blog Features:
#### The GistBlog API has the following blog features:
> - Create new blog post - Allows users to create a new blog post
> - Get all blogs posts - Allows users to get a list of all blog posts
> - Get all users blog post - Allows users to get a list of all their blog posts
> - Get single blog post by id - Allows users to get a single blog post by its ID
> - Update a user blog post - Allows users to update their blog posts
> - Delete a blog post - Allows users to delete their blog posts
> - Blog image upload - Allows users to upload images to their blog posts

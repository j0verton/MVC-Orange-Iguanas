# Tabloid MVC

A website based off of Medium.com, that was created with a group of four people. The purpose of Tabloid was to utilized and level up our understanding of building an application in ASP.NET with the MVC Pattern
So it's time to pivot. We're still going to focus on long-form writing, but not we'll let people write their own posts.

## What's in it??

Full CRUD with intricate foreign key relationships found throughout the database. A Repository Pattern was created to handle all of the SQL commands which allows the users to manipulate the data. 

Models were created to represent the Data Tables for User Profiles, Posts, Comments, Tags, and categories. 

Views utlize Razor pages which combine C# and HTML, which combine to give our users a pleasant way to view data and create/manipulate items which are within their authorization

Controllers were created for each model as a way to handle request coming from the browser, and relayed back to repositories to retrieve or manipulate items. Which then return one of the views so that users have a streamlined experience for creating, reading, updating, or even deleting posts on the website.

### Users

Tabloid Has two types of users, Authors and Admis, which are limited by authorization rights:

* **Authors** can create Posts, manage their own Posts(to include editing and deleting), are prevented from making any changes to Posts which don't belong to them, and read and comment on other authors' posts

* **Admins** can do all the things authors can do, but are also in charge of managing all the data in the system. 

### ERD

![Tabloid ERD](./Tabloid.png)

### Let'stake a visual journey as an Author
![Tabloid Login](./MVC-Orange-Iguanas/TabloidMVC/wwwroot/Tabloid_caption_1.PNG)
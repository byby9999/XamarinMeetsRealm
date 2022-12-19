# DemoRealmTwoDataVersions
 
Demo - how to use Device Sync - Atlas App Services (former Realm Sync) with different data versions.

Key page: SurgeriesPage.xaml + SurgeriesPage.xaml.cs

Models used: Models/genesis-partnercollections-svgjj.cs 

How to use app: 

- click on Login

- Choose a user to login to Realm app

- Observe the object counters

- Try inserting one/more objects, see how counters update.

- Edits and deletes will not affect counters. Delete operation is setting a flag field: IsActive=false. Edits and deletes are affecting only 1 object at a time. App only edits/deletes the most recently added object (with latest CreatedOn value)

- To see how added objects also reflect in the different data version, login as the other user
	- click Logout
	- click Login and choose the other user 

We currently only have 2 users, one for each available data version. If needed, more can be created, they have to be added in App Services - App Users tab

This app works with Email/Password authentication. For the scope of this Demo, JWT auth was not necessary. 
In production project, we will extract the association of user-version from a different place, probably from JWT.
In current state, for simplification, the association is done in variable UsersVersionsMap, in SurgeriesPage.xaml.cs

This simulates the existence of 2 users on the same tenant. 
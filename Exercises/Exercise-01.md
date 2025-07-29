# Exercise 01 - Contiguous IDs

1. Use the "/register" POST endpoint to create 2 Users
2. Login with User1 - ensure "useCookies" and "useSessionCookies" are set to true.
3. Use the "/api/todoitems" POST endpoint to create 3 todo items with different names. When submitting the request, leave the "id" field as 0, and note that the returned todoitems will have incrementing IDs.
4. Login with User2 - ensure "useCookies" and "useSessionCookies" are set to true.
5. Use the "/api/todoitems" POST endpoint to create 3 todo items with different names. When submitting the request, leave the "id" field as 0, and note that the returned todoitems will have incrementing IDs, *starting at the ID after ones created by User1*.
6. Get a todo item using the "/api/todoitems.<id>" GET endpoint. Note that due to a flaw in the system, you can get back a todoitem created by another user.
7. Confirm that the "/api/todoitems" GET endpoint returns a "403 Forbidden" response. *This is expected*. Only administrators should be able to list all cookies?

*Can you get a list of all existing todoitems in the system?*

Hint: Because the IDs are contiguous, if you know todoitems with ids 5 and 7 exist, you can easily assume todoitems with ids 1 - 4 and 6 exist.

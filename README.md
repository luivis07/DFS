FileSize
| KB  | Bytes  |
|-----|--------|
| 75  | 76800  |
| 110 | 112640 |
| 130 | 133120 |
| 200 | 204800 |
| 500 | 512000 |

In the following instructions all paths are relative to the home directory of the solution `DFS.sln`.

The commands should work without any change required if they are executed from `.../DFS`

How to run:
- Build: 
    - `dotnet build dfs.sln`
    - should see message:
    ```
        Build succeeded.
        0 Warning(s)
        0 Error(s)
    ```
- Server: `dotnet run --project dfs.server/dfs.server.console/dfs.server.console.csproj`
    - Should see message:
    ```
    Server starting...Done!
    ```
- Client (normal) : `dotnet run --project dfs.client/dfs.client.console/dfs.client.console.csproj`
    - Should see message, the GUID will change for each client:
    ```
    Starting Session...
    (248844c2-227d-48f0-a799-5e5c6abff758): session established
    ```
    - Followed by list of documents available.
- Client (admin) : `dotnet run --project dfs.client/dfs.client.console/dfs.client.console.csproj admin`
    - Should see message:
    ```
    Starting Session...
    (248844c2-227d-48f0-a799-5e5c6abff758): session established
    ```
    - Followed by commands available.

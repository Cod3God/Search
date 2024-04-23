Version 2: 12-03-2024

A seachengine that consist of an indexer and a search program.

The indexer will crawl a folder (in depth) and create a reverse index
in a database. It will only index text files with .txt as extension.

The search program is a console program that offers a query-based search
in the reverse index. It is in the ConsoleSearch project.

The class library Shared contains classes that are used by the indexer
and the ConsoleSearch. It contains:

- Paths containing static paths for 1) files to index and, 2) a path for
  the database
- BEDocument (BE for Business Entity) - a class representing a document.

The project Renamer is a console program used to rename all files in a
folder. Current version will rename all files with no extension to have
.txt as extension.

Version 2:

1. A project for a webapp to perform searching is added - see WebAppServer.

2. The core search logic is encapsulated in an API - see SearchAPI.

3. The class library Shared is renamed to Core - a much better name.



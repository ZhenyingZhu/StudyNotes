GameData.cs
`public int UpdateGameListFromOwnedPackageInfo( Int64 accountId, SortedSet<int> ignored, AppTypes includedTypes, out int newApps )` read games
1. read from `appcache\packageinfo.vdf`. This file contains all packages
2. read from `userdata\{userid}\config\localconfig.vdf`. This file contains all licenses
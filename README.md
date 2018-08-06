# KeyboardLogic.SolidEdge.AddIn.ItemCatalog

## Debuggin log file locations
- Item Catalog log file (.log): installPath\logs
- Application crash dump file (.dmp): %LOCALAPPDATA%\CrashDumps
- Additional location examples
	C:\Users\eheyder\AppData\Local\Temp\crashlogf.dmp
	C:\Users\eheyder\AppData\Local\Temp\crashlogf.txt

## Developer Notes
In Package Manager Console run the following commands:
- Register application for testing
```
Register-SolidEdgeAddIn
```
- Unregister the application when done
```
Unregister-SolidEdgeAddIn
```

Registery Key information for installer:
Computer\HKEY_CLASSES_ROOT\CLSID\{B440CD64-4926-446A-AA6E-A5115E21F43D}
Computer\HKEY_CURRENT_USER\Software\Unigraphics Solutions\Solid Edge\Version 110\Detail\AddIns\{B440CD64-4926-446A-AA6E-A5115E21F43D}

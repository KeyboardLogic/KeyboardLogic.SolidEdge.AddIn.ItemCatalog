# KeyboardLogic.SolidEdge.AddIn.ItemCatalog

## Install Instructions
1. Download latest zip file
2. Create folder path `C:\Program Files\ItemCatalog`
3. Unzip file to the folder path `C:\Program Files\ItemCatalog`
4. Run `C:\Program Files\ItemCatalog\Registration.bat` with administrator privileges
5. Enter `1` for Registration when prompted on screen
6. Hit any key to close installer

## Uninstall Instructions
1. Run `C:\Program Files\ItemCatalog\Registration.bat` with administrator privileges
2. Enter `2` for Unregister when prompted on screen
3. Hit any key to close installer
4. Delete folder path `C:\Program Files\ItemCatalog`

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

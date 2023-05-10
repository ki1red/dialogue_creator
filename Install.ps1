# Установка зависимостей проекта
dotnet restore

# Сборка проекта
dotnet build -c Release

# Создание папки для установки приложения
$InstallPath = "$PSScriptRoot\Dialogs Creator"
New-Item -ItemType Directory -Path $InstallPath

# Копирование исполняемого файла и всех необходимых файлов проекта в папку установки
Copy-Item -Path "$PSScriptRoot\DialogsCreator\bin\Release\net6.0-windows\*" -Destination $InstallPath -Recurse -Force

# Создание ярлыка для запуска приложения
$WshShell = New-Object -comObject WScript.Shell
$Shortcut = $WshShell.CreateShortcut("$env:PUBLIC\Desktop\Dialogs Creator.lnk")
$Shortcut.TargetPath = "$InstallPath\DialogueCreator.exe"
$Shortcut.WorkingDirectory = $InstallPath
$Shortcut.Save()

# Создание скрипта деинсталляции в папке установки
$UninstallScript = @'
# Удаление ярлыка с рабочего стола
Remove-Item "$env:PUBLIC\Desktop\Dialogs Creator.lnk"

# Удаление всех файлов приложения
Remove-Item * -Recurse -Force
'@

$UninstallScriptPath = Join-Path -Path $InstallPath -ChildPath "Uninstall.ps1"
Set-Content -Path $UninstallScriptPath -Value $UninstallScript

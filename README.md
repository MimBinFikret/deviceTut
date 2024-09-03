# deviceTut

deviceTut is a simple C# console application that uses Windows Management Instrumentation (WMI) to detect and display information about Smart Card Readers connected to the system. It queries the system for all Smart Card Readers and retrieves their DeviceID.

## Features

- Detects all Smart Card Readers connected to the system.
- Displays the DeviceID of each Smart Card Reader.
- Handles exceptions during the querying process.

## Requirements

- .NET Framework or .NET Core installed on your system.
- Windows operating system with WMI support.

## Installation

1. Clone this repository to your local machine or download it as a ZIP file.
2. Open the solution in Visual Studio or your preferred C# development environment.
3. Build the solution.

## Usage

1. Run the application. 
2. The application will search for Smart Card Readers connected to your system.
3. If any Smart Card Readers are found, their DeviceIDs will be displayed.
4. If no Smart Card Readers are found, a message indicating this will be shown.

## Example Output

```
Smart Card Reader DeviceID: USB\VID_1234&PID_5678\6&23ABF3C&0&1
No Smart Card Readers found.
Press Enter to exit...
```

## License

This project is licensed under the T1 License. To obtain the license or for more information, please contact me directly. If I am not reachable, it should be considered that no license has been granted.

## Contributing

If you have any suggestions, improvements, or bug reports, feel free to create a pull request or open an issue. Your contributions are welcome and appreciated.

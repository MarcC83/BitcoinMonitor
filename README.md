# <a href="https://bitcoinmonitor.azurewebsites.net/">Bitcoin Monitor</a>
Blazor app monitoring bitcoin price
The app retreives BTC to EUR price from <a href="https://api.coinbase.com">Coinbase public API</a>. Configurable Sampling interval and a configurable noise is added to the retrieved value. 
To change the application configuration edit <a href="https://github.com/MarcC83/BitcoinMonitor/blob/master/Src/BitcoinMonitor/appsettings.json">appsettings.json</a> or use environment variables as described in <a href="https://github.com/MarcC83/BitcoinMonitor/blob/master/Src/BitcoinMonitor/.env.example">.env.example</a>
    
Application is currently accessible on https://bitcoinmonitor.azurewebsites.net/

## Running the application locally
### With dotnet 6 SDK
Make sure to have .net6 <a href="https://dotnet.microsoft.com/en-us/download/dotnet/6.0">SDK</a> installed. After cloning the project, using terminal from the cloned directory, run the following command
***dotnet run --project Src\BitcoinMonitor\BitcoinMonitor.csproj -c release***  

Application will be accessible on localhost:5011  

### Using docker
#### building the image and running
Using the terminal in the cloned repository root directory run:  
***docker build . -t bitcoinmonitor***

Once the image is built, run:  
***docker run -p 80:80 bitcoinmonitor***   
App will be accessible on localhost:80

### Using image from docker hub 
Use the following command to pull the image from docker hub and run it  
***docker run -p 80:80 marcclapera/bitcoinmonitor:1.0.1***

## Test
To run the unit test, use the command from the cloned repository root directory  
***dotnet test***


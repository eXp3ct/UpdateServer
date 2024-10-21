API_PATH=output/api
WEBUI_PATH=output/webui
API_PORT=5000

start-api:
	cd $(API_PATH) && dotnet YourApi.dll

start-webui:
	cd $(WEBUI_PATH) && serve -s .

start-all: start-api start-webui

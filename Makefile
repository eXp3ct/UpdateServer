API_PATH=./src/Api
WEBUI_PATH=./src/Webui
API_PORT=5069
API_URL=http://localhost:$(API_PORT)

# Определение команды для установки serve на разных ОС
ifeq ($(OS),Windows_NT)
    INSTALL_SERVE=npm install -g serve
else
    INSTALL_SERVE=sudo npm install -g serve
endif

# Запуск API
start-api:
	cd $(API_PATH) && \
	nohup dotnet Api.dll --urls=http://0.0.0.0:$(API_PORT) > api.log 2>&1 & \
	echo "ASP.NET API запущено на порту $(API_PORT)"

# Изменение API_URL в config.json перед запуском WebUI
configure-webui:
	@echo "Обновление API URL в config.json"
	sed -i.bak 's|"API_URL":.*|"API_URL": "$(API_URL)"|' $(WEBUI_PATH)/public/config.json

# Установка serve, если он не установлен
install-serve:
	@if ! command -v serve >/dev/null 2>&1; then \
		echo "Устанавливаю serve..."; \
		$(INSTALL_SERVE); \
	else \
		echo "serve уже установлен"; \
	fi

# Запуск React WebUI
start-webui: configure-webui install-serve
	cd $(WEBUI_PATH) && \
	nohup serve -s build > webui.log 2>&1 & \
	echo "ReactJS запущено на порту 3000"

# Остановка всех процессов
stop:
	pkill -f "dotnet Api.dll" || true
	pkill -f "serve -s" || true
	echo "Все процессы остановлены"

# Показ последних 20 строк логов API и WebUI
logs:
	@echo "=== Логи API ==="
	@tail -n 20 $(API_PATH)/api.log || echo "Лог API недоступен"
	@echo "\n=== Логи React ==="
	@tail -n 20 $(WEBUI_PATH)/webui.log || echo "Лог React недоступен"

# Запуск API и WebUI
start-all: start-api start-webui

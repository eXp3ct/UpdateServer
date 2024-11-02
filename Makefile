# Makefile for Vite + ASP.NET Core project

# Пути и настройки
API_PATH=./api
WEBUI_PATH=./webui
API_PORT=5069
API_URL=http://localhost:$(API_PORT)/api
NODE_ENV ?= development

# Определение ОС
ifeq ($(OS),Windows_NT)
    KILL_API=taskkill /F /IM dotnet.exe || true
    KILL_WEBUI=taskkill /F /IM node.exe || true
else
    KILL_API=pkill -f "dotnet Api.dll" || true
    KILL_WEBUI=pkill -f "vite" || true
endif

.PHONY: install start-api start-webui start-all stop logs clean

# Установка зависимостей
install:
	@echo "Installing API dependencies..."
	cd $(API_PATH) && dotnet restore
	@echo "Installing WebUI dependencies..."
	cd $(WEBUI_PATH) && npm install

# Запуск API
start-api:
	@echo "Starting API on port $(API_PORT)..."
	cd $(API_PATH) && \
	dotnet Api.dll --urls=http://0.0.0.0:$(API_PORT) > api.log 2>&1 & \
	echo "ASP.NET API started on port $(API_PORT)"

# Запуск Vite в режиме разработки
start-webui-dev: export API_URL=$(API_URL)
start-webui-dev: export NODE_ENV=development
start-webui-dev:
	@echo "Starting Vite dev server..."
	cd $(WEBUI_PATH) && \
	npm run dev > webui.log 2>&1 & \
	echo "Vite dev server started"

# Запуск собранного Vite приложения
start-webui-prod: export API_URL=$(API_URL)
start-webui-prod: export NODE_ENV=production
start-webui-prod:
	@echo "Starting production build..."
	cd $(WEBUI_PATH) && \
	npm run preview > webui.log 2>&1 & \
	echo "Production server started"

# Сборка для разных окружений
build-dev: export API_URL=$(API_URL)
build-dev: export NODE_ENV=development
build-dev:
	cd $(WEBUI_PATH) && npm run build -- --mode development

build-prod: export API_URL=$(API_URL)
build-prod: export NODE_ENV=production
build-prod:
	cd $(WEBUI_PATH) && npm run build -- --mode production

# Запуск всего в режиме разработки
start-all-dev: start-api start-webui-dev

# Запуск всего в режиме production
start-all-prod: start-api start-webui-prod

# Остановка всех процессов
stop:
	$(KILL_API)
	$(KILL_WEBUI)
	@echo "All processes stopped"

# Очистка
clean:
	rm -rf $(API_PATH)/bin $(API_PATH)/obj
	rm -rf $(WEBUI_PATH)/dist $(WEBUI_PATH)/node_modules
	rm -f $(API_PATH)/api.log $(WEBUI_PATH)/webui.log

# Показ логов
logs:
	@echo "=== API Logs ==="
	@tail -n 20 $(API_PATH)/api.log || echo "API log not available"
	@echo "\n=== WebUI Logs ==="
	@tail -n 20 $(WEBUI_PATH)/webui.log || echo "WebUI log not available"
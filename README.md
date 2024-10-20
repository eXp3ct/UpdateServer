# UpdateServer
Сервер на базе ASP.NET Core, который предоставляет новые версии программного обеспечения в формате ZIP. Это позволит пользователям автоматически обновлять свои приложения, используя различные библиотеки для автообновления, например AutoUpdater.

# Основные функции
* Обновление программ: Сервер будет хранить ZIP-архивы с новыми версиями приложений, которые могут быть загружены клиентскими приложениями.
* Интерфейс API: Реализация RESTful API для взаимодействия клиентских приложений с сервером. Клиенты будут запрашивать информацию о доступных обновлениях и загружать их.
* Безопасность: Обеспечение безопасного доступа к обновлениям через аутентификацию и авторизацию.

# Технологический стек
* Серверная часть: ASP.NET Core для создания RESTful API.
* База данных: SQLite для хранения информации о версиях программ.

# Пример использования
* Запрос обновления: Клиентское приложение отправляет запрос на сервер для проверки наличия новой версии.
* Ответ сервера: Сервер отвечает с информацией о последней версии и URL к ZIP-файлу.
* Загрузка и установка: Клиент загружает ZIP-файл и устанавливает обновление.
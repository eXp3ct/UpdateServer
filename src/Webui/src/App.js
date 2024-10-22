import React, { useState, useEffect } from 'react';
import { Container, Row, Col, Card, Button } from 'react-bootstrap';
import VersionList from './components/VersionList';
import DynamicModal from './components/VersionModal';
import { fetchAppNames, createVersion, uploadFileToAPI } from './api/api';
import './components/components.css';

function App() {
  const [applications, setApplications] = useState([]);
  const [isModalVisible, setModalVisible] = useState(false);
  const [selectedApp, setSelectedApp] = useState(null);


  const handleAppSelect = (appName) => {
    setSelectedApp((prev) => (prev === appName ? null : appName));
  };



  const [versionData, setVersionData] = useState({
    applicationName: '',
    version: '',
    isMandatory: false,
    isActive: false,
    changelogFile: undefined,
    buildFile: undefined,
  });

  // Получение списка приложений при монтировании компонента
  useEffect(() => {
    const loadApplications = async () => {
      try {
        const apps = await fetchAppNames();
        setApplications(apps);
      } catch (error) {
        console.error('Ошибка при загрузке приложений:', error);
      }
    };

    loadApplications();
  }, []);


  const openModal = () => {
    setVersionData((prev) => ({ ...prev, applicationName: selectedApp }));
    setModalVisible(true);
  };

  const closeModal = () => setModalVisible(false);

  const handleFieldChange = (e, fieldName) => {
    const { type, value, checked, files } = e.target;
    setVersionData((prev) => ({
      ...prev,
      [fieldName]: type === 'checkbox' ? checked : files ? files[0] : value,
    }));
  };

  const handleFormSubmit = async (e) => {
    e.preventDefault();

    try {
      // Создание новой версии
      const versionPayload = {
        applicationName: versionData.applicationName,
        version: versionData.version,
        isMandatory: versionData.isMandatory,
        isActive: versionData.isActive,
        zipUrl: '',
        changelogFileUrl: '',
      };

      const newVersion = await createVersion(versionPayload);

      // Загрузка файлов на API
      if (versionData.changelogFile) {
        await uploadFileToAPI(
          versionData.changelogFile,
          `${newVersion.id}`,
          'changelog'
        );
      }

      if (versionData.buildFile) {
        await uploadFileToAPI(
          versionData.buildFile,
          `${newVersion.id}`,
          'zip'
        );
      }

      console.log('Версия успешно добавлена!');
      closeModal();

      const apps = await fetchAppNames();
      setApplications(apps);
    } catch (error) {
      console.error('Ошибка при добавлении версии:', error);
    }
  };

  return (
    <Container>
      <h1 className="text-center my-4">AutoUpdater Server</h1>
      <Row>
        {applications.map((app) => (
          <Col key={app.appName} xs={4}>
            <Card
              onClick={() => handleAppSelect(app.appName)}
              className={`mb-3 app-button bg-light ${selectedApp === app.appName ? "border-primary" : ""}`}
              style={{ cursor: 'pointer' }}
            >
              <Card.Body>{app.appName}</Card.Body>
            </Card>
          </Col>
        ))}
        <Col xs={4}>
          <Button
            variant="primary"
            className="w-100 mb-3"
            onClick={openModal}
          >
            + Добавить версию
          </Button>
        </Col>
      </Row>

      {selectedApp && <VersionList app={selectedApp} />}


      <DynamicModal
        show={isModalVisible}
        handleClose={closeModal}
        title="Создать новую версию"
        fields={[
          {
            label: 'Приложение',
            type: 'text',
            name: 'applicationName',
            value: versionData.applicationName,
          },
          {
            label: 'Версия',
            type: 'text',
            name: 'version',
            value: versionData.version,
          },
          {
            label: 'Обязательна',
            type: 'checkbox',
            name: 'isMandatory',
            value: versionData.isMandatory,
          },
          {
            label: 'Доступна',
            type: 'checkbox',
            name: 'isActive',
            value: versionData.isActive,
          },
          {
            label: 'Файл изменений',
            type: 'file',
            name: 'changelogFile',
            value: undefined,
          },
          {
            label: 'Файл сборки',
            type: 'file',
            name: 'buildFile',
            value: undefined,
          },
        ]}
        handleInputChange={handleFieldChange}
        handleSubmit={handleFormSubmit}
      />
    </Container>
  );
}

export default App;

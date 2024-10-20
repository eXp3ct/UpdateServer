import React, { useState, useEffect } from 'react';
import { Container, Row, Col, Card, Button, Modal, Form } from 'react-bootstrap';
import VersionList from './VersionList';
import VersionModal from './VersionModal';
import { getAppNames } from './api/api';

// const apps = [
//   { id: 1, name: 'App1' },
//   { id: 2, name: 'App2' },
//   { id: 3, name: 'App3' },
//   { id: 4, name: 'App4' },
//   { id: 5, name: 'App5' },
// ];



function App() {
  const [selectedApp, setSelectedApp] = useState(null);
  const [showVersionModal, setShowVersionModal] = useState(false);
  const [apps, setApps] = useState([])

  // Fetch app names on component mount
  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await getAppNames();
        setApps(response);
      } catch (error) {
        console.error('Error fetching app names:', error);
      }
    };

    fetchData();
  }, []);

  const handleAppClick = (app) => {
    setSelectedApp(app);
  };

  const handleAddVersionClick = () => {
    setShowVersionModal(true);
  };

  const handleCloseModal = () => setShowVersionModal(false);

  return (
    <Container>
      <h1 className="text-center my-4">AutoUpdater Server</h1>
      <Row>
        {apps.map((app) => (
          <Col key={app.appName} xs={4}>
            <Card onClick={() => handleAppClick(app.appName)} className="mb-3">
              <Card.Body>{app.appName}</Card.Body>
            </Card>
          </Col>
        ))}
        <Col xs={4}>
          <Button className="btn w-100" onClick={handleAddVersionClick}>
            + Добавить версию
          </Button>
        </Col>
      </Row>

      {selectedApp && <VersionList app={selectedApp} />}

      <VersionModal
        show={showVersionModal}
        handleClose={handleCloseModal}
        appName={selectedApp ? selectedApp.appName : ''}
      />
    </Container>
  );
}

export default App;

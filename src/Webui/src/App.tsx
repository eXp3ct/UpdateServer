import { useEffect, useState } from 'react';
import { Container, Row, Col, Button } from 'react-bootstrap';
import ApplicationCard from './components/ApplicationCard';
import VersionList from './components/VersionList';
import AddApplicationModal from './components/AddApplicationModal';
import { Application } from './types/types';
import 'bootstrap/dist/css/bootstrap.min.css';
import { createApp, fetchApps } from './api/applicationService';

function App() {
    const [applications, setApplications] = useState<Application[]>([]);
    const [selectedApp, setSelectedApp] = useState<Application | null>(null);
    const [showAddModal, setShowAddModal] = useState(false);

    const handleSelectApp = (app: Application) => {
        setSelectedApp(app);
    };

    useEffect(() => {
      const loadApplications = async () => {
        try{
          const apps = await fetchApps();
          setApplications(apps);
        }catch(error){
          console.error('Cannot load applications', error);
        }
      }

      loadApplications();
    }, []);

    const handleAddApplication = async (name: string, description: string) => {
        const newApp: Application = {
            id: 0,
            name,
            description,
            dateModified: '',
            dateOfCreation: ''
        };

        const addedApp = await createApp(newApp);

        setApplications([...applications, addedApp]);
    };

    const handleAddVersion = (app: Application) => {
        // Здесь будет логика добавления версии
        console.log('Adding version for:', app.name);
    };

    return (
        <Container className="py-4">
            <div className="d-flex justify-content-between align-items-center mb-4">
                <h1>Сохраненные приложения</h1>
                <Button variant="success" onClick={() => setShowAddModal(true)}>
                    Добавить приложение
                </Button>
            </div>

            <Row>
                <Col md={selectedApp ? 6 : 12}>
                    <Row>
                        {applications.map(app => (
                            <Col key={app.id} md={selectedApp ? 12 : 6} lg={selectedApp ? 12 : 4}>
                                <ApplicationCard
                                    application={app}
                                    onSelect={handleSelectApp}
                                />
                            </Col>
                        ))}
                    </Row>
                </Col>
                {selectedApp && (
                    <Col md={6}>
                        <VersionList
                            selectedApp={selectedApp}
                            onAddVersion={handleAddVersion}
                        />
                    </Col>
                )}
            </Row>

            <AddApplicationModal
                show={showAddModal}
                onHide={() => setShowAddModal(false)}
                onAdd={handleAddApplication}
            />
        </Container>
    );
}

export default App;
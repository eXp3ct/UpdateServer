import React from 'react';
import { Card, Button } from 'react-bootstrap';
import { Application } from '../types/types';

interface Props {
    application: Application;
    onSelect: (app: Application) => void;
}

const ApplicationCard: React.FC<Props> = ({ application, onSelect }) => {
    return (
        <Card className="mb-3">
            <Card.Body>
                <Card.Title>{application.name}</Card.Title>
                <Card.Text>{application.description}</Card.Text>
                <Button variant="primary" onClick={() => onSelect(application)}>
                    Посмотреть версии
                </Button>
            </Card.Body>
        </Card>
    );
};

export default ApplicationCard;
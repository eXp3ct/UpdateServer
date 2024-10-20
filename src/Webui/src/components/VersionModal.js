import React from 'react';
import { Modal, Button, Form } from 'react-bootstrap';

function DynamicModal({ 
  show, 
  handleClose, 
  title, 
  fields, 
  handleInputChange, 
  handleSubmit 
}) {
  return (
    <Modal show={show} onHide={handleClose}>
      <Modal.Header closeButton>
        <Modal.Title>{title}</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <Form onSubmit={handleSubmit}>
          {fields.map((field, index) => (
            <Form.Group controlId={field.name} key={index} className="mt-3">
              <Form.Label>{field.label}</Form.Label>
              {field.type === 'file' ? (
                <Form.Control
                  type="file"
                  name={field.name}
                  onChange={(e) => handleInputChange(e, field.name)}
                  accept={field.accept || '*'}
                />
              ) : field.type === 'checkbox' ? (
                <Form.Check
                  type="checkbox"
                  name={field.name}
                  label={field.label}
                  checked={field.value}
                  onChange={(e) => handleInputChange(e, field.name)}
                />
              ) : (
                <Form.Control
                  type={field.type}
                  name={field.name}
                  value={field.value}
                  onChange={(e) => handleInputChange(e, field.name)}
                  placeholder={field.placeholder || ''}
                />
              )}
            </Form.Group>
          ))}
        </Form>
      </Modal.Body>
      <Modal.Footer>
        <Button variant="secondary" onClick={handleClose}>
          Отмена
        </Button>
        <Button variant="primary" onClick={handleSubmit}>
          Сохранить
        </Button>
      </Modal.Footer>
    </Modal>
  );
}

export default DynamicModal;

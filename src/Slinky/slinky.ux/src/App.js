import React, { Component } from 'react';
import { InputGroup, FormControl, Button, Alert, Spinner } from 'react-bootstrap';

export default class App extends Component {
    static displayName = App.name;

    constructor(props) {
        super(props);
        this.state = {
            rawUrl: '',
            lastError: '',
            lastShortlink: '',
            loading: false
        };

        this.reset = this.reset.bind(this);
        this.requestNewShortlink = this.requestNewShortlink.bind(this);
        this.handleRawUrlChanged = this.handleRawUrlChanged.bind(this);
        this.renderUriInput = this.renderUriInput.bind(this);
        this.renderSuccess = this.renderSuccess.bind(this);
        this.renderFailure = this.renderFailure.bind(this);
        this.goNow = this.goNow.bind(this);
    }

    renderUriInput() {
        return (
            <InputGroup className="mb-3" size="lg">
                <InputGroup.Text className="text-muted">https://</InputGroup.Text>
                <FormControl placeholder="yourfancyurl.com" value={this.state.rawUrl} onChange={this.handleRawUrlChanged}/>
                <Button variant="outline-primary" onClick={this.requestNewShortlink}>get your link</Button>
            </InputGroup>
        );
    }

    renderSpinner() {
        return (
            <div className="d-flex justify-content-center">
                <Spinner animation="grow" variant="primary" size="lg" />
            </div>
        );
    }

    renderSuccess() {
        return (
            <Alert key="success" variant="success" size="lg">
                <div className="d-flex justify-content-end">
                    <p className="flex-fill mb-0 align-self-center">Congratulations! Your fancy new shortlink is: <a href="{this.state.lastShortlink}"><strong>{this.state.lastShortlink}</strong></a></p>
                    <Button className="ms-1" variant="outline-success" onClick={this.reset}>shorten again</Button>
                </div>
            </Alert>
        );
    }

    renderFailure(message, app) {
        return (
            <Alert key="danger" variant="danger" size="lg">
                <div className="d-flex justify-content-end">
                    <p className="flex-fill mb-0 align-self-center">{this.state.lastError}</p>
                    <Button variant="outline-danger" onClick={this.reset}>try again</Button>
                </div>
            </Alert>
        );
    }

    reset() {
        this.setState({
            rawUrl: '',
            lastError: '',
            lastShortlink: '',
            loading: false
        });
    }

    handleRawUrlChanged(e) {
        this.setState({
            rawUrl: e.target.value
        });
    }

    goNow() {
        window.location.href = this.state.lastShortlink;
    }

    requestNewShortlink(e) {
        this.setState({
            lastError: '',
            lastShortlink: '',
            loading: true
        });

        const body = {
            Uri: `https://${this.state.rawUrl}`
        };

        const options = {
            method: 'POST',
            headers: {
                'accept': 'text/plain',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(body),
        };

        fetch('shortlink', options)
            .then(response => {
                if (!response.ok) {
                    return response.json()
                        .then(error => {
                            throw new Error(`${error.error} (HTTP ${response.status})`);
                        });
                }
                return response.json();
            })
            .then(data => {
                this.setState({
                    rawUrl: '',
                    lastError: '',
                    lastShortlink: data.shortlink,
                    loading: false
                });
            })
            .catch(error => {
                this.setState({
                    lastError: `An error occurred: ${error.message}`,
                    lastShortlink: '',
                    loading: false
                });
            });
    }

    render() {
        let content;
        if (!this.state.loading) {
            if (this.state.lastError == null || this.state.lastError.trim() === '') {
                if (this.state.lastShortlink == null || this.state.lastShortlink.trim() === '') {
                    content = this.renderUriInput();
                } else {
                    content = this.renderSuccess();
                }
            } else {
                content = this.renderFailure();
            }
        } else {
            content = this.renderSpinner();
        }

        return (
            <div className="container py-4">
                <div className="container d-flex flex-column justify-content-center">
                    <h1 className="display-1 text-center p-4 pacifico">Slinky</h1>
                    <div>
                        {content}
                    </div>
                    <div className="px-5 mb-4 bg-light rounded-3">
                        <div className="container-fluid py-5">
                            <p className="fs-5">Slinky is a shortlinking app built using React and ASP.NET Core 6. Caching is implemented with the cache-aside pattern, backed by Redis, and data is persisted using SQL Server. Load balancing is deferred to NGINX.</p>
                            <p className="fs-5">It aims to demonstrate the principles involved in designing a distributed, scaleable web-application characterised by high-availability.</p>
                            <p className="fs-5">This application is <a href="https://code.leif.uk/lwg/scaledgo">open source software</a>, and I invite you to learn from it however is most effective for you.</p>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}

import React, { Component } from 'react';
import { withRouter } from './WithRouter.js'

class Redirector extends Component {
    static displayName = Redirector.name;

    constructor(props) {
        super(props);
        const { id } = this.props.params;
        this.state = { id: id, success: true }
    }

    componentDidMount() {
        this.fetchRealLink(this.state.id);
    }

    fetchRealLink(id) {
        const options = {
            method: 'POST',
            headers: {
                'accept': 'text/plain',
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({}),
        };

        fetch(`/api/v1/shortlink/${id}`, options)
            .then(response => {
                if (!response.ok) {
                    this.setState({ success: false });
                    return;
                }

                return response.json();
            })
            .then(data => {
                if (data != null) {
                    window.location.href = data.uri;
                }
            });
    }

    render() {
        let content = this.state.success ?
            <h1>Redirecting you now...</h1>
            : <h3>An unspecified error occurred during redirection. Are you sure that was a valid shortlink?</h3>

        return (
            <div className="container d-flex align-items-center justify-content-center">
                {content}
            </div>
        );
    }

}

export default withRouter(Redirector);

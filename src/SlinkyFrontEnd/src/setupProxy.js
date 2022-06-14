const { createProxyMiddleware } = require('http-proxy-middleware');

const context = [
    "/api/v1",
];

module.exports = function (app) {
    const appProxy = createProxyMiddleware(context, {
        target: 'http://localhost:5239/',
        secure: false
    });

    app.use(appProxy);
};

const { createProxyMiddleware } = require('http-proxy-middleware');

const context = [
    "/shortlink",
];

module.exports = function (app) {
    const appProxy = createProxyMiddleware(context, {
        target: 'http://localhost:5239/api/v1',
        secure: false
    });

    app.use(appProxy);
};

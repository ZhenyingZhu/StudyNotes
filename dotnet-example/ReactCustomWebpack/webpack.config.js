const webpack = require('webpack');
const path = require('path');

const APP_DIR = path.resolve(__dirname, 'ClientApp');
const PUBLIC_DIR = path.resolve(__dirname, 'public');

const config = {
    entry: APP_DIR + '/Client.js',
    output: {
        path: PUBLIC_DIR,
        filename: 'bundle.js',
        publicPath: '/'
    },
    devServer: {
        contentBase: PUBLIC_DIR,
        port: 9000,
        open: true
    },
    devtool: 'source-map',
    module: {
        rules: [
            {
                test: /\.js?$/,
                loader: 'babel-loader',
                exclude: /node_modules/,
                options: {
                    presets: [
                        'react',
                        'stage-2',
                        ['env', {targets: {browsers: ['last 2 versions']}}]
                    ]
                }
            }
        ]
    },
    mode: 'development'
};

module.exports = config;
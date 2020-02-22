const webpack = require('webpack');
const path = require('path');

const APP_DIR = path.resolve(__dirname, 'ClientApp');
const PUBLIC_DIR = path.resolve(__dirname, 'public');
const BUILD_DIR = path.resolve(__dirname, 'build');

const config = {
    entry: APP_DIR + '/Client.js',
    devServer: {
        contentBase: PUBLIC_DIR,
        port: 9000,
        open: true,
        histroyApiFallback: true
    },
    output: {
        path: PUBLIC_DIR,
        filename: 'bundle.js'
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
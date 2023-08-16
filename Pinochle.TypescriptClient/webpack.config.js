const path = require('path');
module.exports = {
    entry: './src/PinochleJs.ts',
    devtool: 'inline-source-map',
    module: {
        rules: [{
            test: /\.tsx?$/,
            use: 'ts-loader',
            exclude: /node_modules/
        }]
    },
    resolve: {
        extensions: ['.ts', '.js', '.tsx']
    },
    output: {
        filename: 'pinochlejs.js',
        path: path.resolve(__dirname, 'dist'),
        library: 'pinochle-js',
        libraryTarget: 'umd'
    }
};
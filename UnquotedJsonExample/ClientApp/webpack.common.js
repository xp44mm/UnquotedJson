const path = require('path')
const { CleanWebpackPlugin } = require('clean-webpack-plugin')
const HtmlWebPackPlugin = require('html-webpack-plugin')

module.exports = {
    entry: './index.js',

    output: {
        filename: '[name].[contenthash].js',
        path: path.resolve(__dirname, 'dist'),
    },

    resolve: {
        extensions: ['.js', '.json'],
        alias: {
            'rxjs': path.resolve(__dirname, 'node_modules', 'rxjs'),
            'structural-comparison': path.resolve(__dirname, 'node_modules', 'structural-comparison'),

        },

    },

    module: {
        rules: [
            {
                test: /\.js$/,
                exclude: [/[\\/]node_modules[\\/]/],
                use: [
                    {
                        loader: 'babel-loader',
                        options: { babelrc: true },
                    },
                ],
            },
            {
                test: /\.css$/,
                exclude: [/[\\/]node_modules[\\/]/],
                use: [
                    { loader: 'style-loader' },
                    { loader: 'css-loader', },
                ],
            },
            {
                test: /\.html$/,
                exclude: [/[\\/]node_modules[\\/]/],
                use: [
                    { loader: 'html-loader', },
                ],
            },
            {
                test: /\.(png|svg|jpg|jpeg|gif)$/i,
                exclude: [/[\\/]node_modules[\\/]/],
                type: "asset"
            },

        ],
    },

    plugins: [
        new CleanWebpackPlugin(),
        new HtmlWebPackPlugin({
            template: './assets/index.html',
            favicon: './assets/favicon.ico',
            filename: 'index.html'
        }),
    ],
}

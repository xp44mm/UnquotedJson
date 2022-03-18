const { merge } = require('webpack-merge')
const common = require('./webpack.common.js')

module.exports = merge(common, {
    mode: 'development',
    devtool: 'eval-source-map',
    devServer: {
        port: 3000,
        historyApiFallback: true,
        allowedHosts: "all",
    },

    //optimization: {
    //   runtimeChunk: 'single',
    //   splitChunks: {
    //      chunks: 'all',
    //   },
    //}

})

const { merge } = require('webpack-merge');
const common = require('./webpack.common.js');

module.exports = merge(common, {
    mode: 'production',
    output: {
        filename: 'unquoted-json.js',
        library: 'unquotedJson', // window.unquotedJson
        libraryTarget: 'umd',
        globalObject: 'this',
    },

})

module.exports = function (api) {
    //api.assertVersion(7);
    api.cache.using(() => process.env.NODE_ENV)
    //api.cache(true)

    let presets = [
        [
            '@babel/preset-env',
            {
                targets: api.env('test') ? { node: 'current' } : { chrome: '91' },

                //targets: { chrome: '86', node: 'current' },

                loose: true, // convert from es6 to es5 loosely.
                corejs: 3, //declaring a core-js version
                useBuiltIns: 'entry',
            },
        ],
    ]
    let plugins = [
        '@babel/plugin-proposal-export-default-from',
        '@babel/plugin-proposal-export-namespace-from',
        '@babel/plugin-proposal-class-properties',
        ['@babel/plugin-proposal-pipeline-operator', { proposal: 'fsharp' }],
        //"@babel/plugin-proposal-partial-application",
        //'@babel/plugin-proposal-nullish-coalescing-operator',
        //'@babel/plugin-proposal-optional-chaining',
        //'@babel/plugin-proposal-do-expressions',(()=>{})();

    ]

    return { presets, plugins }
}

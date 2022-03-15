let defaultConfig = {
   printWidth: 80,
   tabWidth: 2,
   useTabs: false,
   semi: true,
   singleQuote: false,
   quoteProps: 'as-needed',
   trailingComma: 'none',
   bracketSpacing: true,
   arrowParens: 'avoid',
   proseWrap: 'preserve',
   htmlWhitespaceSensitivity: 'css',
   endOfLine: 'auto',
}

let advanceDefaultConfig = {
   rangeStart: 0,
   rangeEnd: Infinity,
   parser: null,
   filepath: null,
   requirePragma: false,
   insertPragma: false,

   jsxSingleQuote: false,
   jsxBracketSameLine: false,
   vueIndentScriptAndStyle: false,
}

//Object.assign({}, defaultConfig, )

module.exports = {
   printWidth: 140,
   tabWidth: 4,
   semi: false,
   trailingComma: 'es5',
   singleQuote: true,
   //endOfLine: 'crlf',
   //proseWrap: 'always',
}

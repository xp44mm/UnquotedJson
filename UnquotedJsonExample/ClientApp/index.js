import './index.css'

import { fragment } from 'hyperscript-rxjs'
import { element } from './src'


let elem = element()

document.addEventListener('DOMContentLoaded', function () {
    const root = document.getElementById('root')
    let el = elem instanceof Array ? fragment(...elem) : elem
    root.appendChild(el)
})


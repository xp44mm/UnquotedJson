import { AbortController, AbortSignal } from "abort-controller"
import realFetch from 'node-fetch'

if (!global.AbortController) {
    global.AbortController = AbortController
}

if (!global.AbortSignal) {
    global.AbortSignal = AbortSignal
}

if (!global.fetch) {
    global.fetch = realFetch
}

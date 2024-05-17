# Freedeck Handoff

Freedeck Handoff allows browsers to interact with the Freedeck Host using the Handoff API (introduced in v6).  

It uses a URL protocol (`freedeck://`).  

How to use handoff:

`freedeck://action/app_id/extra_data`

## Using Handoff API

To use the Handoff API (for your own Handoff), you must first obtain a token by going to `http://localhost:5754/handoff/get-token`.
This token can only be grabbed once per minute, and changes every minute.

So far, there is only one supported method: reloading plugins.
`http://localhost:5754/handoff/TOKEN_HERE/reload-plugins` - This will reload the plugin cache. When the client reloads it will be as if the user pressed Reload in the app iteslf.

## Downloading from Website v2

Website v2 introduced the ability to download plugins directly to your Freedeck with the click of a button. The URL used is formatted like so:

`freedeck://download/plugin_id/plugin_url`

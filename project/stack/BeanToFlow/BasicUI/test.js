[
    {
        "id": "587a2cae.423595",
        "type": "tab",
        "label": "text3",
        "disabled": false,
        "info": ""
    },
    {
        "id": "4712de51.6e0f1",
        "type": "ui_text_input",
        "z": "587a2cae.423594",
        "name": "Text3",
        "label": "Text3",
        "tooltip": "",
        "group": "49822746.4c5e18",
        "order": 1,
        "width": 0,
        "height": 0,
        "passthru": true,
        "mode": "text",
        "delay": "10",
        "topic": "TextInput",
        "x": 490,
        "y": 260,
        "wires": [
            [
                "6d52323.87d2fcc",
                "2e2df341.83f3fc"
            ]
        ]
    },
    {
        "id": "1f33bb7f.d09ac5",
        "type": "function",
        "z": "587a2cae.423594",
        "name": "Clear",
        "func": "msg.payload = null;\nreturn msg;",
        "outputs": 1,
        "noerr": 0,
        "x": 250,
        "y": 260,
        "wires": [
            [
                "4712de51.6e0f1"
            ]
        ]
    },
    {
        "id": "88a187a1.78fba8",
        "type": "function",
        "z": "587a2cae.423594",
        "name": "Set_Text",
        "func": "console.log(msg);\nreturn msg;",
        "outputs": 1,
        "noerr": 0,
        "x": 260,
        "y": 380,
        "wires": [
            [
                "4712de51.6e0f1"
            ]
        ]
    },
    {
        "id": "6d52323.87d2fcc",
        "type": "function",
        "z": "587a2cae.423594",
        "name": "TextChanged",
        "func": "if(msg.topic==\"TextChanged\")    \n    msg.payload = \"textChanged\";\nreturn msg;",
        "outputs": 1,
        "noerr": 0,
        "x": 770,
        "y": 260,
        "wires": [
            [
                "d7865809.ed8cf8"
            ]
        ]
    },
    {
        "id": "2e2df341.83f3fc",
        "type": "function",
        "z": "587a2cae.423594",
        "name": "EventGetPropRet",
        "func": "if(msg.topic == \"GetProp\"){\n    return msg;\n}\n",
        "outputs": 1,
        "noerr": 0,
        "x": 790,
        "y": 380,
        "wires": [
            [
                "827532aa.1cd36"
            ]
        ]
    },
    {
        "id": "dbe3bbe6.5b1c28",
        "type": "link in",
        "z": "587a2cae.423594",
        "name": "",
        "links": [],
        "x": 75,
        "y": 260,
        "wires": [
            [
                "1f33bb7f.d09ac5"
            ]
        ]
    },
    {
        "id": "1933f7da.e25758",
        "type": "link in",
        "z": "587a2cae.423594",
        "name": "",
        "links": [],
        "x": 75,
        "y": 380,
        "wires": [
            [
                "88a187a1.78fba8"
            ]
        ]
    },
    {
        "id": "bdefd93.11c2828",
        "type": "function",
        "z": "587a2cae.423594",
        "name": "GetProp",
        "func": "msg.topic = \"GetProp\";\nreturn msg;",
        "outputs": 1,
        "noerr": 0,
        "x": 260,
        "y": 500,
        "wires": [
            [
                "2e2df341.83f3fc"
            ]
        ]
    },
    {
        "id": "108f8605.b47aea",
        "type": "link in",
        "z": "587a2cae.423594",
        "name": "",
        "links": [
            "9a8d50cd.30d8b"
        ],
        "x": 75,
        "y": 500,
        "wires": [
            [
                "bdefd93.11c2828"
            ]
        ]
    },
    {
        "id": "d7865809.ed8cf8",
        "type": "link out",
        "z": "587a2cae.423594",
        "name": "",
        "links": [
            "2317b85e.6334b8"
        ],
        "x": 920,
        "y": 260,
        "wires": []
    },
    {
        "id": "827532aa.1cd36",
        "type": "link out",
        "z": "587a2cae.423594",
        "name": "",
        "links": [
            "22c7b96b.c07316"
        ],
        "x": 880,
        "y": 460,
        "wires": []
    },
    {
        "id": "49822746.4c5e18",
        "type": "ui_group",
        "z": "",
        "name": "Elements of entries",
        "tab": "b0a3a364.b9c8f",
        "order": 1,
        "disp": true,
        "width": "6",
        "collapse": false
    },
    {
        "id": "b0a3a364.b9c8f",
        "type": "ui_tab",
        "z": "",
        "name": "Tab 2",
        "icon": "dashboard",
        "order": 2,
        "disabled": false,
        "hidden": false
    }
]
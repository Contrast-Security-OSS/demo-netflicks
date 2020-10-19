import yaml, json
with open('./contrast_security.yaml') as f:
    config = yaml.load(f)
    print(json.dumps(config['api']))
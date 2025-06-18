FROM mcr.microsoft.com/playwright:v1.50.0

WORKDIR /tests
COPY package.json package-lock.json /tests/

# Install dependencies from the package-lock.json file above
RUN npm ci

# Add the base playwright config. This will need to be overwritten with a volume if changes are needed. 
COPY playwright.config.ts /tests/playwright.config.ts
COPY global-setup.ts /tests/global-setup.ts

# Add example test for testing the container. Will be overwritten with the actual tests via volumes.
COPY . /tests

# Run playwright test
ENV BASEURL="http://demo-netflicks-web-1"
ENV CI=true
CMD [ "npx", "playwright", "test", "assess" ]


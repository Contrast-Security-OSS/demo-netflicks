FROM mcr.microsoft.com/playwright:v1.32.1-jammy
# copy project (including tests)
# COPY . /tests

WORKDIR /tests
# COPY ./tests/package.json ./tests/package-lock.json /tests/
COPY package.json /tests/

# Install dependencies from the package-lock.json file above
# RUN npm ci
RUN npm install
# Install browsers - TODO: Do we need this line? 
# RUN npx playwright install

# Add the base playwright config. This will need to be overwritten with a volume if changes are needed. 
COPY playwright.config.ts /tests/playwright.config.ts
COPY global-setup.ts /tests/global-setup.ts

# Add example test for testing the container. Will be overwritten with the actual tests via volumes.
COPY tests /tests

# Run playwright test
ENV BASEURL="http://demo-netflicks-web-1"
CMD [ "npx", "playwright", "test", "assess" ]
# EXPOSE 9323
# CMD ["/bin/bash"]

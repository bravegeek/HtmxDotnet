module.exports = {
  transform: {
    '^.+\\.[tj]sx?$': 'ts-jest', // Use ts-jest for type checking
  },
  moduleFileExtensions: ['ts', 'tsx', 'js', 'jsx'],
  testEnvironment: 'node',
  roots: ['<rootDir>/Tests'],
};

interface IPool<T> {
  Rent(): T;
  zero(): void;
}

interface IPool<T> {
  Rent(): T;
  Zero(): void;
}

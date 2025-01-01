interface IGameEvents {
  upSpecial: number;
  downSpecial: number;
  sideSpecial: number;
  special: number;
  upAttack: number;
  downAttack: number;
  sideAttcak: number;
  attack: number;
  idle: number;
  run: number;
  jump: number;
  grab: number;
  guard: number;
}

export const Actions: IGameEvents = {
  upSpecial: 0,
  downSpecial: 1,
  sideSpecial: 2,
  special: 3,
  upAttack: 4,
  downAttack: 5,
  sideAttcak: 6,
  attack: 7,
  idle: 8,
  run: 9,
  jump: 10,
  grab: 11,
  guard: 12,
};

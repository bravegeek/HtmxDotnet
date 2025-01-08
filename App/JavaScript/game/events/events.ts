interface IGameEvents {
  //user inputs
  upSpecial: number;
  downSpecial: number;
  sideSpecial: number;
  special: number;
  upAttack: number;
  downAttack: number;
  sideAttcak: number;
  attack: number;
  idle: number;
  move: number;
  moveFast: number;
  jump: number;
  grab: number;
  guard: number;
  //Altered User Events
  turn: number;
  // engin events
  land: number;
}

export const GameEvents: IGameEvents = {
  // user inputs
  upSpecial: 0,
  downSpecial: 1,
  sideSpecial: 2,
  special: 3,
  upAttack: 4,
  downAttack: 5,
  sideAttcak: 6,
  attack: 7,
  idle: 8,
  move: 9,
  moveFast: 10,
  jump: 11,
  grab: 12,
  guard: 13,
  //Altered user events
  turn: 14,
  // engine events
  land: 15,
};

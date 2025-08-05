interface Props {
  delta: number;
  pos: number;
}

const PositionDelta: React.FC<Props> = ({ delta, pos }) => (
  <div className="flex flex-col items-center justify-center gap-1 text-[0.65rem] font-bold">
    <div className="text-sm font-bold">{pos}</div>
    {delta < 0 && (
      <div className="flex flex-row items-center justify-items-center text-red-700 dark:text-red-400">
        <div className="flex flex-col items-center">
          <div>{delta * -1}</div>
          <svg
            style={{ marginTop: -4 }}
            xmlns="http://www.w3.org/2000/svg"
            fill="none"
            viewBox="0 0 24 24"
            strokeWidth="4"
            stroke="currentColor"
            className="h-2.5 w-2.5 md:h-4 md:w-4"
          >
            <path
              strokeLinecap="round"
              strokeLinejoin="round"
              d="M19.5 8.25l-7.5 7.5-7.5-7.5"
            />
          </svg>
        </div>
      </div>
    )}
    {delta > 0 && (
      <div className="flex items-center justify-items-center text-green-700 dark:text-green-400">
        <div className="flex flex-col items-center">
          <div style={{ marginBottom: -4 }}>
            <svg
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 24 24"
              strokeWidth="4"
              stroke="currentColor"
              className="h-2.5 w-2.5 md:h-4 md:w-4"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                d="m4.5 15.75 7.5-7.5 7.5 7.5"
              />
            </svg>
          </div>
          <div>{delta}</div>
        </div>
      </div>
    )}
    {delta === 0 && <div>-</div>}
  </div>
);

export default PositionDelta;

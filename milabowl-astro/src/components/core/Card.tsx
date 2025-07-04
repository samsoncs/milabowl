export interface Props {
  title?: string;
  secondary?: React.ReactNode;
  children?: React.ReactNode;
}

const Card2: React.FC<Props> = ({ children, title, secondary }) => (
  <div className="space-y-2 rounded-md bg-white p-4 shadow-sm dark:bg-slate-800 dark:shadow-sm-dark">
    <div className="flex">
      <div className="mb-3 grow text-xl font-bold text-indigo-900 dark:text-dark-text">
        {title}
      </div>
      {secondary}
    </div>

    <div className="overflow-x-auto text-base dark:text-dark-text">
      {children}
    </div>
  </div>
);

export default Card2;

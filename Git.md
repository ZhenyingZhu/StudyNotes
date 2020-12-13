# Git

[Git remove a local commit](http://stackoverflow.com/questions/5097456/throw-away-local-commits-in-git)

[merge git commits into one](http://gitready.com/advanced/2009/02/10/squashing-commits-with-rebase.html)

[Create a branch from another local repo](http://stackoverflow.com/questions/10603671/git-how-to-add-a-local-repo-and-treat-it-as-a-remote-one)

[count number of lines](http://stackoverflow.com/questions/26881441/can-you-get-the-number-of-lines-of-code-from-a-github-repository)

[take mine when merge conflict](https://stackoverflow.com/questions/914939/simple-tool-to-accept-theirs-or-accept-mine-on-a-whole-file-using-git)

[revert a merge](https://stackoverflow.com/questions/7099833/how-to-revert-a-merge-commit-thats-already-pushed-to-remote-branch)

[git rebase](https://git-scm.com/book/en/v2/Git-Branching-Rebasing)
Make the master branch as base, and then apply your changes on it one commit by one.

Before merging master, better push local branch to remote, so that if merging master causing issue, I can reset back to remote branch and have all my changes.

Create a folder in remote: make the branch name as: folder/branch

[git blame a line](https://stackoverflow.com/questions/13692072/git-blame-committed-line)

`git gc` garbage collection. Might need to run `git prune`.

`git fsck` see nasty commits.

`git config --global gc.auto 0` set disable auto GC.

`git checkout branch -- file` get a file from another branch.

Check which commit deleted a line: `git log -S "deleted line" file`

`git checkout -b branch_name origin/branch_name` to set the track

`git branch --set-upstream-to=origin/[branch] [branch]`

To force commit the local change to remote, such as after done a rebase, do `git push -f origin <branch>`

Uncommit a local change

- If need the history: `git revert <commit id>`
- If need files in the last commit: `git reset --soft HEAD~1`
- If don't need files: `git reset --hard HEAD~1`

To sync a remote branch:

`git update-ref -d refs/remotes/origin/<branch>`

[Git Submodules](https://git-scm.com/book/en/v2/Git-Tools-Submodules)

- `git submodule add <sub repo url>`
- After clone a project with submodule, run `git submodule init`
- `git push command takes the --recurse-submodules=check` to push changes in submodules as well.
- [Specify Branch](https://stackoverflow.com/questions/1777854/how-can-i-specify-a-branch-tag-when-adding-a-git-submodule#:~:text=git%20submodule%20add%20-b%20is%20not%20some%20magically,commit%20of%20a%20specified%20branch%20before%20populating%20it.)

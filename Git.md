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
- to keep the folder update to date: `git submodule update --remote`. This actually check in the commit id.

Under `.git\hooks` folder, can write a pre-commit and a post-commit sh script.

`git rev-parse --show-toplevel` returns the root of the git repo folder.

`git -am "msg"` can add and commit together (Hum? Seems like not working, maybe add an [alias](https://stackoverflow.com/questions/2419249/how-can-i-stage-and-commit-all-files-including-newly-added-files-using-a-singl)).

`git submodule` delete the folder to resolve un-commited changes.

git stuck at auto packing the repository for optimum performance

- <https://stackoverflow.com/questions/28633956/why-does-git-keep-telling-me-its-auto-packing-the-repository-in-background-for>
- <https://gist.github.com/xiaoda/2d1ae0417d48dcbd972aafcdc098b4b6>

[git rev-list](https://git-scm.com/docs/git-rev-list)

- find the commits since a commit reversely.

`git clean -f -d` to stop pulling the packages.

## Git not remember user name

[Git does not remember username and password on Windows](https://snede.net/git-does-not-remember-username-password/#:~:text=Git%20does%20not%20remember%20username%20and%20password%20on,%E2%80%9Crun%20command%E2%80%9D%20and%20open%20the%20key%20manager%20)

[Getting Git To Remember Your Username and Password](https://xp-dev.com/docs/user-guide/repositories/remember-username-password.html#:~:text=To%20get%20your%20Git%20client%20to%20remember%20your,you%20can%20do%20so%20by%20specifying%20a%20--timeout%3D%3A): not natively working on windows

[Error fatal: credential-cache unavailable; no Unix socket support](https://stackoverflow.com/questions/67951554/error-fatal-credential-cache-unavailable-no-unix-socket-support)

<https://stackoverflow.com/questions/77287038/how-to-manage-github-credentials-with-windows-10#:~:text=Run%20git%20config%20--get%20credential.helper%2C%20then%20depending%20on,erase%20wincred%3A%20run%20the%20following%20command%3A%20cmdkey%20%2Fdelete%3ALegacyGeneric%3Atarget%3Dgit%3Ahttps%3A%2F%2Fgithub.com>

[git clone with different username/account](https://stackoverflow.com/questions/39644366/git-clone-with-different-username-account)

- When cloning: `git clone https://ZhenyingZhu@github.com/ZhenyingZhu/StudyNotes.git`

## Git RPC failure

Error message:

```powershell
error: RPC failed; curl 56 HTTP/2 stream 7 was reset
send-pack: unexpected disconnect while reading sideband packet
fatal: the remote end hung up unexpectedly
```

Fix: <https://gist.github.com/daopk/0a95772d582cafb202142ff7871da2fc>

## CRLF cannot commit

<https://stackoverflow.com/questions/15467507/trying-to-commit-git-files-but-getting-fatal-lf-would-be-replaced-by-crlf-in>